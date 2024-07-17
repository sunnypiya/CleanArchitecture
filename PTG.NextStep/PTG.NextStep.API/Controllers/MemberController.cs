using Microsoft.AspNetCore.Mvc;
using PTG.NextStep.API.MockData;
using PTG.NextStep.Database.Search;
using PTG.NextStep.Service;
using PTG.NextStep.Domain;
using PTG.NextStep.Domain.Models;
using PTG.NextStep.Domain.DTO;
using FluentValidation;

namespace PTG.NextStep.API.Controllers
{
	[Route("api/v1/[controller]")]
	[ApiController]
	public class MemberController : ControllerBase
	{
		private readonly ILogger<MemberController> _logger;
		private readonly ISearchService _searchService;
		private readonly IMemberService _memberService;
		private readonly IValidationDictionary _validationDictionary;
		private readonly IValidator<MemberBasicDTO> _validatorMemberBasic;
		private readonly IValidator<MemberContactDTO> _validatorMemberContact;

		public MemberController(ILogger<MemberController> logger,
			ISearchService searchService,
			IMemberService memberService,
			IValidationDictionary validationDictionary
			, IValidator<MemberBasicDTO> validatorMemberBasic
			, IValidator<MemberContactDTO> validatorMemberContact)
		{

			_logger = logger;
			_searchService = searchService;
			_memberService = memberService;
			_validationDictionary = validationDictionary;
			_validatorMemberBasic = validatorMemberBasic;
			_validatorMemberContact = validatorMemberContact;
		}

		[HttpGet("{employeeNumber}")]
		public async Task<IActionResult> GetMemberByEmployeeNumberAsync(string employeeNumber)
		{
			try
			{
				_logger.LogInformation("GetMemberByEmployeeNumberAsync called");
				var member = await _memberService.GetMemberBasicDetailsByEmployeeNumberAsync(employeeNumber);
				if (member != null)
					return Ok(member);
				else
					return NotFound("Member not found");
			}
			catch (Exception ex)
			{
				_logger.LogError($"Error in method:GetMemberByEmployeeNumberAsync: Message: {ex.Message}", ex);
				return BadRequest(new
				{
					Error = "Error while fetching member detail.",
					//Message = ex.Message
				});
			}
		}

		/// <summary>
		/// Update Member Basic details
		/// </summary>
		/// <param name="memberContact"></param>
		/// <returns></returns>
		[HttpPut("save-memberbasicinfo")]
		public async Task<IActionResult> SaveMemberBasicInfo(MemberBasicDTO memberBasic) //Validation needs to be revisited and common middleware or filter need to be used
		{
			try
			{
				_logger.LogInformation("SaveMemberBasicInfo() called");
				var result = await _validatorMemberBasic.ValidateAsync(memberBasic);
				if (!result.IsValid)
				{
					return BadRequest(new
					{
						Errors = result.Errors.Where(e => e.Severity == Severity.Error)
								.Select(e => e.ErrorMessage)
								.ToArray(),
						Warnings = result.Errors.Where(e => e.Severity == Severity.Warning)
								.Select(e => e.ErrorMessage)
								.ToArray()
					});
				}
				else
				{
					var isSaved = await _memberService.SaveMemberBasicInfoAsync(memberBasic, _validationDictionary);
					if (isSaved)
					{
						var response = new { Message = "Member Basic Record Saved Successfully." };
						return new JsonResult(response)
						{
							ContentType = "application/json"
						};
					}
					else
					{
						return BadRequest(new
						{
							Errors = _validationDictionary.Errors.Values.ToArray(),
							Warnings = _validationDictionary.Warnings.Values.ToArray()
						});
					}
				}
			}
			catch (Exception ex)
			{
				_logger.LogError($"Error in method:SaveMemberBasicInfo: Message:{ex.Message}", ex);
				return BadRequest(new
				{
					Error = "Error while saving Member Basic detail."
				});
			}
		}

		[HttpGet("contactinfo/{link}")]
		public async Task<IActionResult> GetMemberContactInfoByLinkAsync(decimal link)
		{
			try
			{
				_logger.LogInformation("GetMemberContactInfoByLinkAsync() called");
				var memberContact = await _memberService.GetMemberContactByLinkAsync(link);
				if (memberContact != null)
					return Ok(memberContact);
				else
					return NotFound("Member Contact Info not found for the given link");
			}
			catch (Exception ex)
			{
				_logger.LogError($"Error in method:GetMemberContactInfoByLinkAsync: {ex.Message}", ex);
				return BadRequest(new
				{
					Error = "Error while fetching Member Contact Info detail.",
					//Message = ex.Message
				});
			}
		}

		/// <summary>
		/// Update Member Contact details
		/// </summary>
		/// <param name="memberContact"></param>
		/// <returns></returns>
		[HttpPut("contactinfo")]
		public async Task<IActionResult> UpdateMemberContactInfo([FromBody] MemberContactDTO memberContact)
		{
			try
			{
				_logger.LogInformation("UpdateMemberContactInfo() called");
				var result = await _validatorMemberContact.ValidateAsync(memberContact);
				if (!result.IsValid)
				{
					return BadRequest(new
					{
						Errors = result.Errors.Where(e => e.Severity == Severity.Error)
								.Select(e => e.ErrorMessage)
								.ToArray(),
						Warnings = result.Errors.Where(e => e.Severity == Severity.Warning)
								.Select(e => e.ErrorMessage)
								.ToArray()
					});
				}
				else
				{
					var isSaved = await _memberService.SaveMemberContactInfoAsync(memberContact, _validationDictionary);

					if (isSaved)
					{
						var response = new { Message = "Member Contact Record Updated Successfully." };
						return new JsonResult(response)
						{
							ContentType = "application/json"
						};
					}
					else
					{
						return BadRequest(new
						{
							Errors = _validationDictionary.Errors.Values.ToArray(),
							Warnings = _validationDictionary.Warnings.Values.ToArray()
						});
					}
				}
			}
			catch (Exception ex)
			{
				_logger.LogError($"Error in method:UpdateMemberContactInfo: {ex.Message}", ex);
				return BadRequest(new
				{
					Error = "Error while saving Member Contact detail."
					//Message = ex.Message
				});
			}
		}

		/// <summary>
		/// Create Member Contact details
		/// </summary>
		/// <param name="memberContact"></param>
		/// <returns></returns>
		[HttpPost("contactinfo")]
		public async Task<IActionResult> CreateMemberContactInfo(MemberContactDTO memberContact)
		{
			try
			{
				_logger.LogInformation("CreateMemberContactInfo() called");
				var result = await _validatorMemberContact.ValidateAsync(memberContact);
				if (!result.IsValid)
				{
					return BadRequest(new
					{
						Errors = result.Errors.Where(e => e.Severity == Severity.Error)
								.Select(e => e.ErrorMessage)
								.ToArray(),
						Warnings = result.Errors.Where(e => e.Severity == Severity.Warning)
								.Select(e => e.ErrorMessage)
								.ToArray()
					});
				}
				else
				{
					var isCreated = await _memberService.CreateMemberContactInfoAsync(memberContact, _validationDictionary);

					if (isCreated)
					{
						var response = new { Message = "Member Contact Record Created Successfully." };
						return new JsonResult(response)
						{
							ContentType = "application/json"
						};
					}
					else
					{
						return BadRequest(new
						{
							Errors = _validationDictionary.Errors.Values.ToArray(),
							Warnings = _validationDictionary.Warnings.Values.ToArray()
						});
					}
				}
			}
			catch (Exception ex)
			{
				_logger.LogError($"Error in method:CreateMemberContactInfo: {ex.Message}", ex);
				return BadRequest(new
				{
					Error = "Error while creating Member Contact detail.",
					//Message = ex.Message
				});
			}
		}


		[HttpGet("statushistory/{link}")]
		public async Task<IActionResult> GetMemberStatusHistoryByLinkAsync(decimal link)
		{
			try
			{
				_logger.LogInformation("GetMemberStatusHistoryByLinkAsync() called");
				var memberContactHistory = await _memberService.GetMemberStatusHistoryByLinkAsync(link);
				return Ok(memberContactHistory);
			}
			catch (Exception ex)
			{
				_logger.LogError($"Error in method:GetMemberStatusHistoryByLinkAsync: {ex.Message}", ex);
				return BadRequest(new
				{
					Error = "Error while fetching Member Status Service Info detail.",
					//Message = ex.Message
				});
			}
		}

		[HttpPost("statushistory")]
		public async Task<IActionResult> AddMemberStatusHistory(MemberStatusHistoryDTO memberStatusHistory)
		{
			try
			{
				_logger.LogInformation("AddMemberStatusHistory() called");
				var memberContact = await _memberService.AddMemberStatusHistory(memberStatusHistory, _validationDictionary);
				if (memberContact)
				{
					var response = new { Message = "Member Status History Record Added Successfully." };
					return new JsonResult(response)
					{
						ContentType = "application/json"
					};
				}
				else
					return BadRequest("Error while adding Member Status History Record");
			}
			catch (Exception ex)
			{
				_logger.LogError($"Error in method:AddMemberStatusHistory: {ex.Message}", ex);
				return BadRequest(new
				{
					Error = "Error while adding Member Status Info detail.",
					//Message = ex.Message
				});
			}
		}

		[HttpGet("servicehistory/{link}")]
		public async Task<IActionResult> GetMemberServiceHistoryByLinkAsync(decimal link)
		{
			try
			{
				_logger.LogInformation("GetMemberServiceHistoryByLinkAsync() called");
				var memberServiceHistory = await _memberService.GetMemberServiceHistoryByLinkAsync(link);
				return Ok(memberServiceHistory);
			}
			catch (Exception ex)
			{
				_logger.LogError($"Error in method:GetMemberServiceHistoryByLinkAsync: {ex.Message}", ex);
				return BadRequest(new
				{
					Error = "Error while fetching Member Service Service Info detail.",
					// Message = ex.Message
				});
			}
		}

		[HttpPost("servicehistory")]
		public async Task<IActionResult> AddMemberServiceHistory(MemberServiceHistoryDTO memberServiceHistory) // this object needs to be changed with DTO object
		{
			try
			{
				_logger.LogInformation("AddMemberServiceHistory() called");
				var memberService = await _memberService.AddMemberServiceHistory(memberServiceHistory, _validationDictionary);
				if (memberService)
				{
					var response = new { Message = "Member Service History Record Added Successfully." };
					return new JsonResult(response)
					{
						ContentType = "application/json"
					};
				}
				else
					return BadRequest("Error while adding Member Service History Record");
			}
			catch (Exception ex)
			{
				_logger.LogError($"Error in method:AddMemberServiceHistory: {ex.Message}", ex);
				return BadRequest(new
				{
					Error = "Error while adding Member Service History Info detail.",
					//Message = ex.Message
				});
			}
		}

		[HttpGet("deductions-earnings-history/{link}")]
		public async Task<IActionResult> GetMemberDedsEarnsHistoryByLinkAsync(decimal link)
		{
			try
			{
				_logger.LogInformation("GetMemberDedsEarnsHistoryByLinkAsync() called");
				var memberDedsEarnsHistories = await _memberService.GetMemberDedEarnsHistoryByLinkAsync(link);
				return Ok(memberDedsEarnsHistories);
			}
			catch (Exception ex)
			{
				_logger.LogError($"Error in method:GetMemberDedsEarnsHistoryByLinkAsync: {ex.Message}", ex);
				return BadRequest(new
				{
					Error = "Error while fetching Member Deductions Earnings History Info.",
					//Message = ex.Message
				});
			}
		}

		[HttpGet("search")]
		public async Task<IActionResult> SearchData(string keyword)
		{
			try
			{
				_logger.LogInformation("SearchData() called");

				var searchResponse = await _searchService.SearchData(keyword);

				return Ok(searchResponse);
			}
			catch (Exception ex)
			{
				_logger.LogError($"Error in method:SearchData: {ex.Message}", ex);
				return BadRequest(new
				{
					Error = "Error while Searching Data.",
					// Message = ex.Message
				});
			}
		}

		[HttpGet("earnings-summary/{link}")]
		public async Task<IActionResult> GetEarningsSummaryByLinkAsync(decimal link)
		{
			try
			{
				_logger.LogInformation("GetEarningsSummaryyByLinkAsync() called");
				var earningsSummary = await _memberService.GetEarningsSummaryByLinkAsync(link);
				return Ok(earningsSummary);
			}
			catch (Exception ex)
			{
				_logger.LogError($"Error in method:GetEarningsSummaryByLinkAsync: {ex.Message}", ex);
				return BadRequest(new
				{
					Error = "Error while fetching Earnings Summary Info detail.",
					//Message = ex.Message
				});
			}
		}

		[HttpGet("beneficiaries/{link}")]
		public async Task<IActionResult> GetBeneficiaryInfoList(decimal link)
		{
			try
			{
				_logger.LogInformation("GetBeneficiaryInfoList() called");
				var beneficiaryInfoList = await _memberService.GetBeneficiaryInfoList(link);
				if (beneficiaryInfoList != null)
					return Ok(beneficiaryInfoList);
				else
					return NotFound("Beneficiary Info List not found");
			}
			catch (Exception ex)
			{
				_logger.LogError($"Error in method:GetBeneficiaryInfoList: {ex.Message}", ex);
				return BadRequest(new
				{
					Error = "Error while fetching Beneficiary Info List.",
					//Message = ex.Message
				});
			}
		}

		[HttpGet("power-of-attorney/{link}")]
		public async Task<IActionResult> GetPowerofAttorneyByLinkAsync(decimal link)
		{
			try
			{
				_logger.LogInformation("GetPowerofAttorneyByLinkAsync() called");
				var powerofAttorney = await _memberService.GetPowerofAttorneyByLinkAsync(link);
				return Ok(powerofAttorney);
			}
			catch (Exception ex)
			{
				_logger.LogError($"Error in method:GetPowerofAttorneyByLinkAsync: {ex.Message}", ex);
				return BadRequest(new
				{
					Error = "Error while fetching Power of Attorney Info detail.",
					//Message = ex.Message
				});
			}
		}

		[HttpGet("dependents-info/{link}")]
		public async Task<IActionResult> GetMemberDependentsInfoListByLinkAsync(decimal link)
		{
			try
			{
				_logger.LogInformation("GetMemberDependentsInfoByLinkAsync() called");
				var memberDependents = await _memberService.GetMemberDependentsInfoListByLinkAsync(link);
				return Ok(memberDependents);
			}
			catch (Exception ex)
			{
				_logger.LogError($"Error in method:GetMemberDependentsInfoByLinkAsync: {ex.Message}", ex);
				return BadRequest(new
				{
					Error = "Error while fetching Member Dependents Info.",
					//Message = ex.Message
				});
			}
		}

		[HttpGet("dro-info/{link}")]
		public async Task<IActionResult> GetMemberDROInfoByLinkAsync(decimal link)
		{
			try
			{
				_logger.LogInformation("GetMemberDROInfoByLinkAsync() called");
				var memberDRO = await _memberService.GetMemberDROInfoByLinkAsync(link);
				if (memberDRO != null)
					return Ok(memberDRO);
				else
				{
					var response = new { message = "Member DRO Info not found for the given link" };
					return NotFound(response);
				}
			}
			catch (Exception ex)
			{
				_logger.LogError($"Error in method:GetMemberDROInfoByLinkAsync: {ex.Message}", ex);
				return BadRequest(new
				{
					Error = "Error while fetching Member DRO Info.",
					//Message = ex.Message
				});
			}
		}
	}
}
