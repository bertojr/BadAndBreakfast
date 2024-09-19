import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AdditionalServiceManagementRoutingModule } from './additional-service-management-routing.module';
import { AdditionalServiceManagementComponent } from './additional-service-management.component';
import { AdditionalServiceListComponent } from './additional-service-list/additional-service-list.component';
import { AdditionalServiceEditComponent } from './additional-service-edit/additional-service-edit.component';
import { FormsModule } from '@angular/forms';

@NgModule({
  declarations: [
    AdditionalServiceManagementComponent,
    AdditionalServiceListComponent,
    AdditionalServiceEditComponent,
  ],
  imports: [
    CommonModule,
    AdditionalServiceManagementRoutingModule,
    FormsModule,
  ],
})
export class AdditionalServiceManagementModule {}
