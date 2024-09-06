import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { UserManagementComponent } from './user-management.component';
import { UserEditComponent } from './user-edit/user-edit.component';

const routes: Routes = [
  { path: '', component: UserManagementComponent },
  { path: 'create', component: UserEditComponent },
  { path: ':id', component: UserEditComponent },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class UserManagementRoutingModule {}
