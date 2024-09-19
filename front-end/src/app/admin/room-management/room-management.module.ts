import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { RoomManagementRoutingModule } from './room-management-routing.module';
import { RoomManagementComponent } from './room-management.component';
import { RoomListComponent } from './room-list/room-list.component';
import { RoomEditComponent } from './room-edit/room-edit.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

@NgModule({
  declarations: [RoomManagementComponent, RoomListComponent, RoomEditComponent],
  imports: [
    CommonModule,
    RoomManagementRoutingModule,
    ReactiveFormsModule,
    FormsModule,
  ],
})
export class RoomManagementModule {}
