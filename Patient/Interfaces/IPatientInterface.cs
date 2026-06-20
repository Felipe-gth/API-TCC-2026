
﻿using Api.Patient.DTOs.Edit;
using Api.Patient.DTOs.List;
using Api.Patient.DTOs.Register;
using Api.Shared.DTOs.Result;
using Api.User.DTOs.Email;
using Api.User.DTOs.Phone;
using Api.User.DTOs.Return;
using Api.User.DTOs.Return.Address;
using Api.User.DTOs.Return.Email;
using Api.User.DTOs.Return.Phone;
using Api.User.DTOs.Address;

namespace Api.Patient.Interfaces;

public interface IPatientInterface
{
    Task<Result<ReturnUserDTO>> CreatePatientAsync(RegisterPatientDTO dto);
    Task<Result<ReturnUserDTO>> GetPatientByIdAsync(int id);
    Task<Result<IEnumerable<ListPatientDTO>>> ListPatient();
}