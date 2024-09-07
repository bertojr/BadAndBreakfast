import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AmenityManagementComponent } from './amenity-management.component';

const routes: Routes = [{ path: '', component: AmenityManagementComponent }];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AmenityManagementRoutingModule { }
