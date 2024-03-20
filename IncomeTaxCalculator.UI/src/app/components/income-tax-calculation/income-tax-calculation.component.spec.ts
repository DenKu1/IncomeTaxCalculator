import { TestBed } from '@angular/core/testing';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { of, throwError } from 'rxjs';
import { ToastrService } from 'ngx-toastr';
import { IncomeTaxCalculationComponent } from './income-tax-calculation.component';
import { IncomeTaxService } from '../../services/income-tax.service';

describe('IncomeTaxCalculationComponent', () => {
  let fixture;
  let component;
  let mockIncomeTaxService;
  let mockToastrService;

  beforeEach(() => {
    mockIncomeTaxService = jasmine.createSpyObj(['calculateIncomeTax']);
    mockToastrService = jasmine.createSpyObj(['error']);

    TestBed.configureTestingModule({
      imports: [FormsModule, ReactiveFormsModule],
      declarations: [IncomeTaxCalculationComponent],
      providers: [
        { provide: IncomeTaxService, useValue: mockIncomeTaxService },
        { provide: ToastrService, useValue: mockToastrService }
      ]
    });

    fixture = TestBed.createComponent(IncomeTaxCalculationComponent);
    component = fixture.componentInstance;
  });

  it('should create component', () => {
    expect(component).toBeTruthy();
  });

  it('should calculate income tax', () => {
    component.grossAnnualSalaryFormControl.setValue(1000);
    mockIncomeTaxService.calculateIncomeTax.and.returnValue(of({}));
    component.calculateIncomeTax();
    expect(mockIncomeTaxService.calculateIncomeTax).toHaveBeenCalled();
  });

  it('should not trigger calculation if form is invalid', () => {
    component.grossAnnualSalaryFormControl.setValue(0);
    component.calculateIncomeTax();
    expect(mockIncomeTaxService.calculateIncomeTax).toHaveBeenCalledTimes(0);
  });

  it('should handle error when calculate income tax service call fails', () => {
    component.grossAnnualSalaryFormControl.setValue(1000);
    mockIncomeTaxService.calculateIncomeTax.and.returnValue(throwError({ error: { message: 'Error' } }));
    component.calculateIncomeTax();
    expect(mockToastrService.error).toHaveBeenCalledWith('Error. Error');
  });

  it('should update incomeTaxOutputInfo on service success', () => {
    component.grossAnnualSalaryFormControl.setValue(1000);
    let serviceResponse = {
      grossAnnualSalary: 1000,
      grossMonthlySalary: 200,
      netAnnualSalary: 800,
      netMonthlySalary: 100,
      annualTaxPaid: 200,
      monthlyTaxPaid: 20
    };
    mockIncomeTaxService.calculateIncomeTax.and.returnValue(of(serviceResponse));
    component.calculateIncomeTax();
    expect(component.incomeTaxOutputInfo).toEqual(serviceResponse);
  });
});
