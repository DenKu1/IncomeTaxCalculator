import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { IncomeTaxCalculationComponent } from './components/income-tax-calculation/income-tax-calculation.component';
import { NotFoundComponent } from './components/not-found/not-found.component';

const routes: Routes = [
  {
    path: '',
    redirectTo: '/taxes/calculate-income-tax',
    pathMatch: 'full'
  },
  {
    path: 'taxes/calculate-income-tax',
    component: IncomeTaxCalculationComponent
  },
  {
    path: '**',
    component: NotFoundComponent
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {
}
