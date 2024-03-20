import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { IncomeTaxService } from './income-tax.service';
import { CalculateIncomeTaxRequestModel } from '../models/income-tax-calculation/calculateIncomeTaxRequestModel';
import { CalculateIncomeTaxResponseModel } from '../models/income-tax-calculation/calculateIncomeTaxResponseModel';
import { ResponseModel } from '../models/common/responseModel';
import { environment } from '../../enviroments/environment';

describe('IncomeTaxService', () => {
  let service: IncomeTaxService;
  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [IncomeTaxService]
    });

    service = TestBed.get(IncomeTaxService);
    httpMock = TestBed.get(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should return calculated tax', () => {
    const mockRequest: CalculateIncomeTaxRequestModel = {
      grossAnnualSalary: 40_000
    };

    const responseModel: CalculateIncomeTaxResponseModel = {
      grossAnnualSalary: 40_000,
      grossMonthlySalary: 40_000 / 12,
      netAnnualSalary: 29_000,
      netMonthlySalary: 29_000 / 12,
      annualTaxPaid: 11_000,
      monthlyTaxPaid: 11_000 / 12
    }

    const mockResponse: ResponseModel = {
      result: responseModel,
      isSuccess: true,
      message: ''
    };

    service.calculateIncomeTax(mockRequest).subscribe(res => {
      expect(res.grossAnnualSalary).toEqual(40_000);
    });

    const req = httpMock.expectOne(`${environment.incomeTaxCalculatorApiUrl}/taxes/income-tax/calculate`);
    expect(req.request.method).toBe('POST');
    req.flush(mockResponse);
  });

  it('should throw an error for failed requests', () => {
    const mockRequest: CalculateIncomeTaxRequestModel = {
      grossAnnualSalary: -100
    };

    const mockErrorResponse: ResponseModel = {
      result: null,
      isSuccess: false,
      message: 'Error'
    };

    service.calculateIncomeTax(mockRequest).subscribe(
      () => fail('Should have failed with some error'),
      (error) => expect(error.message).toEqual('Error')
    );

    const req = httpMock.expectOne(`${environment.incomeTaxCalculatorApiUrl}/taxes/income-tax/calculate`);
    expect(req.request.method).toBe('POST');
    req.flush(mockErrorResponse);
  });
});
