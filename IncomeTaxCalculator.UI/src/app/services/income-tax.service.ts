import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, map } from 'rxjs';
import { environment } from '../../enviroments/environment';
import { ResponseModel } from '../models/common/responseModel';
import { CalculateIncomeTaxRequestModel } from '../models/income-tax-calculation/calculateIncomeTaxRequestModel';
import { CalculateIncomeTaxResponseModel } from '../models/income-tax-calculation/calculateIncomeTaxResponseModel';

@Injectable({
  providedIn: 'root'
})
export class IncomeTaxService {
  constructor(private http: HttpClient)
  {
  }
  
  calculateIncomeTax(request: CalculateIncomeTaxRequestModel): Observable<CalculateIncomeTaxResponseModel> {
    return this.http.post<ResponseModel>(`${environment.incomeTaxCalculatorApiUrl}/taxes/income-tax/calculate`, request)
      .pipe(map(response => {
        if (response.isSuccess) {
          return response.result as CalculateIncomeTaxResponseModel;
        } else {
          throw new Error(response.message);
        }
      }));
  }
}
