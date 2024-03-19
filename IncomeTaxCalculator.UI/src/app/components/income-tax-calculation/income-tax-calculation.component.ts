import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { CalculateIncomeTaxRequestModel } from '../../models/income-tax-calculation/calculateIncomeTaxRequestModel';
import { CalculateIncomeTaxResponseModel } from '../../models/income-tax-calculation/calculateIncomeTaxResponseModel';
import { IncomeTaxService } from '../../services/income-tax.service';

@Component({
  selector: 'app-income-tax-calculation',
  templateUrl: './income-tax-calculation.component.html',
  styleUrls: ['./income-tax-calculation.component.css']
})
export class IncomeTaxCalculationComponent {
  
  incomeTaxOutputInfo: CalculateIncomeTaxResponseModel;
  incomeTaxCalculationForm: IncomeTaxCalculationForm;

  constructor(
    private router: Router,
    private activeRoute: ActivatedRoute,
    private formBuilder: FormBuilder,
    private incomeTaxService: IncomeTaxService,
    private toastrService: ToastrService) {
    this.incomeTaxCalculationForm = new IncomeTaxCalculationForm(this.formBuilder);
  }

  calculateIncomeTax(): void {
    if (this.incomeTaxCalculationForm.form.invalid) {
      return;
    }

    this.incomeTaxCalculationForm.startLoading();

    var requestModel: CalculateIncomeTaxRequestModel =
    {
      grossAnnualSalary: this.incomeTaxCalculationForm.f.grossAnnualSalary.value
    }

    this.incomeTaxService.calculateIncomeTax(requestModel)
      //.pipe(first())
      .subscribe(
        response => {
          this.incomeTaxOutputInfo = response; // Update output

          this.incomeTaxCalculationForm.stopLoading();
        },
        err => {
          this.toastrService.error("Error! " + err.message);
          this.incomeTaxCalculationForm.stopLoading();
        });
  }
}

class IncomeTaxCalculationForm {
  isLoading: boolean = false;

  form: FormGroup;

  get f() { return this.form.controls; }

  constructor(private formBuilder: FormBuilder) {
    this.form = this.formBuilder.group({
      grossAnnualSalary: ['', [Validators.required, Validators.min(1)]]
    });
  }

  startLoading(): void {
    this.isLoading = true;
  } 

  stopLoading(): void {
    this.isLoading = false;
    
    this.form.markAsUntouched();
  }  
}
