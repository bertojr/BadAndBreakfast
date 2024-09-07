import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { RoleManagementRoutingModule } from './role-management-routing.module';
import { RoleManagementComponent } from './role-management.component';
import { RoleListComponent } from './role-list/role-list.component';
import { RoleEditComponent } from './role-edit/role-edit.component';


@NgModule({
  declarations: [
    RoleManagementComponent,
    RoleListComponent,
    RoleEditComponent
  ],
  imports: [
    CommonModule,
    RoleManagementRoutingModule
  ]
})
export class RoleManagementModule { }
