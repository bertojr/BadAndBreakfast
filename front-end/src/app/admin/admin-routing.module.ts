import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AdminComponent } from './admin.component';
import { AuthGuard } from '../auth/auth.guard';

const routes: Routes = [
  { path: '', component: AdminComponent },
  {
    path: 'admin/users',
    loadChildren: () =>
      import('./user-management/user-management.module').then(
        (m) => m.UserManagementModule
      ),
    canActivate: [AuthGuard],
  },
  {
    path: 'admin/rooms',
    loadChildren: () =>
      import('./room-management/room-management.module').then(
        (m) => m.RoomManagementModule
      ),
    canActivate: [AuthGuard],
  },
  {
    path: 'admin/amenities',
    loadChildren: () =>
      import('./amenity-management/amenity-management.module').then(
        (m) => m.AmenityManagementModule
      ),
    canActivate: [AuthGuard],
  },
  {
    path: 'admin/additional-services',
    loadChildren: () =>
      import(
        './additional-service-management/additional-service-management.module'
      ).then((m) => m.AdditionalServiceManagementModule),
    canActivate: [AuthGuard],
  },
  {
    path: 'admin/roles',
    loadChildren: () =>
      import('./role-management/role-management.module').then(
        (m) => m.RoleManagementModule
      ),
    canActivate: [AuthGuard],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class AdminRoutingModule {}
