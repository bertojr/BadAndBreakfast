import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { RoomManagementComponent } from './room-management.component';
import { RoomEditComponent } from './room-edit/room-edit.component';

const routes: Routes = [
  { path: '', component: RoomManagementComponent },
  { path: 'create', component: RoomEditComponent },
  { path: ':id', component: RoomEditComponent },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class RoomManagementRoutingModule {}
