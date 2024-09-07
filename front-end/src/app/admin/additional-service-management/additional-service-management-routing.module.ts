import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AdditionalServiceManagementComponent } from './additional-service-management.component';
import { AdditionalServiceEditComponent } from './additional-service-edit/additional-service-edit.component';

const routes: Routes = [
  { path: '', component: AdditionalServiceManagementComponent },
  { path: 'create', component: AdditionalServiceEditComponent },
  { path: ':id', component: AdditionalServiceEditComponent },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class AdditionalServiceManagementRoutingModule {}
