import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AmenityManagementComponent } from './amenity-management.component';
import { AmenityEditComponent } from './amenity-edit/amenity-edit.component';

const routes: Routes = [
  { path: '', component: AmenityManagementComponent },
  { path: 'create', component: AmenityEditComponent },
  { path: ':id', component: AmenityEditComponent },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class AmenityManagementRoutingModule {}
