import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AmenityManagementRoutingModule } from './amenity-management-routing.module';
import { AmenityManagementComponent } from './amenity-management.component';
import { AmenityListComponent } from './amenity-list/amenity-list.component';
import { AmenityEditComponent } from './amenity-edit/amenity-edit.component';


@NgModule({
  declarations: [
    AmenityManagementComponent,
    AmenityListComponent,
    AmenityEditComponent
  ],
  imports: [
    CommonModule,
    AmenityManagementRoutingModule
  ]
})
export class AmenityManagementModule { }
