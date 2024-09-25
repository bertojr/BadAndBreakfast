import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomePageComponent } from './pages/home-page/home-page.component';
import { RoomsPageComponent } from './pages/rooms-page/rooms-page.component';
import { ContactPageComponent } from './pages/contact-page/contact-page.component';
import { BookingsComponent } from './pages/bookings/bookings.component';
import { AuthGuard } from './auth/auth.guard';

const routes: Routes = [
  { path: '', component: HomePageComponent },
  { path: 'bookings', component: BookingsComponent },
  { path: 'rooms', component: RoomsPageComponent },
  { path: 'contact', component: ContactPageComponent },
  {
    path: 'auth',
    loadChildren: () => import('./auth/auth.module').then((m) => m.AuthModule),
  },
  {
    path: 'admin',
    loadChildren: () =>
      import('./admin/admin.module').then((m) => m.AdminModule),
    canActivate: [AuthGuard],
    canActivateChild: [AuthGuard],
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
