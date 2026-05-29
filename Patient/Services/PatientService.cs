using System.Runtime.InteropServices.JavaScript;
using Api.Patient.Data.InterfaceSql;
using Api.Patient.DTOs.Edit;
using Api.Patient.DTOs.List;
using Api.Patient.DTOs.Register;
using Api.Patient.DTOs.Result;
using Api.Patient.Interfaces;
using Api.User.DTOs.Adress;
using Api.User.DTOs.Email;
using Api.User.DTOs.Phone;
using Api.User.DTOs.Return;
using Api.User.DTOs.Return.Adress;
using Api.User.DTOs.Return.Email;
using Api.User.DTOs.Return.Phone;
using Api.User.Models;

namespace tcc.Patient.Services;

public class PatientService : IPatientInterface
{
    private readonly IPatientInterfaceSql _patientSQL;
    public PatientService(IPatientInterfaceSql patient)
    {
        _patientSQL = patient;
    }

    //Register

    public async Task<Result<ReturnUserDTO>> CreatePatientAsync(RegisterPatientDTO dto)
    {
        var user = new UserModel(0, dto.Name, dto.LastName, dto.CPF, dto.Age, dto.Password, "C");
        int data = await _patientSQL.CreatePatientAsync(user);
        if (data <= 0)
        {
            var result = new Result<ReturnUserDTO>
            {
                Sucess = false,
                Data = null

            };
            return result;
        }
        var returndto = new ReturnUserDTO(data, "", "C");
        return new Result<ReturnUserDTO>
        {
            Sucess = true,
            Data = returndto
        };
    }

    //Login


    public async Task<Result<AdressReturnDTO>> CreateAdressAsync(AdressEntryDTO dto)
    {
        var adress = new AdressModel(dto);
        int data = await _patientSQL.CreateAdressAsync(adress);
        if (data <= 0)
        {
            var result = new Result<AdressReturnDTO>
            {
                Sucess = false,
                Data = null
            };
            return result;
        }
        var returnDTO = new AdressReturnDTO(data, adress.CEP, adress.CCity, adress.CState, adress.CNumber, adress.IsApartment, adress.ApartmentNumber);
        return new Result<AdressReturnDTO>
        {
            Sucess = true,
            Data = returnDTO
        };
    }

    public async Task<Result<PhoneNumberReturnDTO>> CreatePhoneNumberAsync(PhoneNumberEntryDTO dto)
    {
        var number = new NumberModel(dto.Id, dto.Number, dto.CountryCode, dto.DDD, dto.IsEmergencyContact);
        int data = await _patientSQL.CreatePhoneNumberAsync(number);
        if (data <= 0)
        {
            return new Result<PhoneNumberReturnDTO>
            {
                Sucess = false,
                Data = null
            };
        }
        var returnDTO = new PhoneNumberReturnDTO(data, number.NNumber, number.NDDD);
        return new Result<PhoneNumberReturnDTO>
        {
            Sucess = true,
            Data = returnDTO
        };
    }

    public async Task<Result<EmailReturnDTO>> CreateEmailAsync(EmailEntryDTO dto)
    {
        var email = new EmailModel(dto.Id, dto.Address, dto.Extension);
        int data = await _patientSQL.CreateEmailAsync(email);
        if (data <= 0)
        {
            return new Result<EmailReturnDTO>
            {
                Sucess = false,
                Data = null
            };
        }
        var returnDTO = new EmailReturnDTO(data, email.EAddress, email.EExtension);
        return new Result<EmailReturnDTO>
        {
            Sucess = true,
            Data = returnDTO
        };
    }

    public async Task<Result<ReturnUserDTO>> GetPatientByIdAsync(int id)
    {
        var patient = await _patientSQL.GetPatientFromIdAsync(id);
        if (patient != null)
        {
            return new Result<ReturnUserDTO>
            {
                Sucess = true,
                Data = patient
            };
        }
        return new Result<ReturnUserDTO>
        {
            Sucess = false,
            Data = null
        };
    }

    public async Task<Result<IEnumerable<ListPatientDTO>>> ListPatient()
    {
        var result = await _patientSQL.ListAllPatient();
        if (result != null && result.Any())
        {
            var a = new Result<IEnumerable<ListPatientDTO>>
            {
                Sucess = true,
                Data = result
            };
            return a;
        }
        else
        {
            var a = new Result<IEnumerable<ListPatientDTO>>
            {
                Sucess = false,
                Data = null
            };
            return a;
        }
    }
    
    public async Task<Result<bool>> EditPatientAsync(EditPatientDTO dto)
    {
        var patient = new UserModel(dto.Id, dto.Name, dto.LastName, dto.CPF, dto.Age, dto.Password, "C");
        bool data = await _patientSQL.EditPatientAsync(patient);
        if (data)
        {
            return new Result<bool>
            {
                Sucess = true,
                Data = true
            };
        }
        return new Result<bool>
        {
            Sucess = false,
            Data = false
        };
    }
}