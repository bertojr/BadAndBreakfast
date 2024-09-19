import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AdminComponent } from './admin.component';

const routes: Routes = [
  { path: '', component: AdminComponent },
  {
    path: 'admin/user',
    loadChildren: () =>
      import('./user-management/user-management.module').then(
        (m) => m.UserManagementModule
      ),
  },
  {
    path: 'admin/room',
    loadChildren: () =>
      import('./room-management/room-management.module').then(
        (m) => m.RoomManagementModule
      ),
  },
  {
    path: 'admin/amenity',
    loadChildren: () =>
      import('./amenity-management/amenity-management.module').then(
        (m) => m.AmenityManagementModule
      ),
  },
  {
    path: 'admin/additional-service',
    loadChildren: () =>
      import(
        './additional-service-management/additional-service-management.module'
      ).then((m) => m.AdditionalServiceManagementModule),
  },
  {
    path: 'admin/role',
    loadChildren: () =>
      import('./role-management/role-management.module').then(
        (m) => m.RoleManagementModule
      ),
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class AdminRoutingModule {}
