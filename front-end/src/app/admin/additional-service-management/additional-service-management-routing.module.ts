import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AdditionalServiceManagementComponent } from './additional-service-management.component';

const routes: Routes = [{ path: '', component: AdditionalServiceManagementComponent }];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AdditionalServiceManagementRoutingModule { }
