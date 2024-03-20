import { Component } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { ResponseModel } from '../../models/common/responseModel';
import { CalculateIncomeTaxRequestModel } from '../../models/income-tax-calculation/calculateIncomeTaxRequestModel';
import { CalculateIncomeTaxResponseModel } from '../../models/income-tax-calculation/calculateIncomeTaxResponseModel';
import { IncomeTaxService } from '../../services/income-tax.service';

@Component({
  selector: 'app-income-tax-calculation',
  templateUrl: './income-tax-calculation.component.html',
  styleUrls: ['./income-tax-calculation.component.css']
})
export class IncomeTaxCalculationComponent {
  grossAnnualSalaryFormControl = new FormControl('', [Validators.required, Validators.min(1)]);
  incomeTaxOutputInfo: CalculateIncomeTaxResponseModel;

  constructor(
    private incomeTaxService: IncomeTaxService,
    private toastrService: ToastrService) {
  }

  calculateIncomeTax(): void {
    if (this.grossAnnualSalaryFormControl.invalid) {
      return;
    }

    this.grossAnnualSalaryFormControl.disable();

    let grossAnnualSalary = Number(this.grossAnnualSalaryFormControl.value);
    let request = this.createCalculateIncomeTaxRequestModel(grossAnnualSalary);

    this.incomeTaxService.calculateIncomeTax(request)
      .subscribe(
        response => {
          this.incomeTaxOutputInfo = response;

          this.grossAnnualSalaryFormControl.enable();
        },
        err => {
          let apiError: ResponseModel = err.error;

          this.toastrService.error("Error! " + apiError.message);
          this.grossAnnualSalaryFormControl.enable();
        });
  }

  createCalculateIncomeTaxRequestModel(grossAnnualSalary: number): CalculateIncomeTaxRequestModel {
    var requestModel: CalculateIncomeTaxRequestModel =
    {
      grossAnnualSalary: grossAnnualSalary
    }

    return requestModel;
  }
}
