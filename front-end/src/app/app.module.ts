import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import {
  HTTP_INTERCEPTORS,
  provideHttpClient,
  withInterceptorsFromDi,
} from '@angular/common/http';
import { AuthInterceptor } from './auth/auth.interceptor';
import { HttpErrorInterceptor } from './interceptor/http-error.interceptor';
import { AdminModule } from './admin/admin.module';
import { HomePageComponent } from './pages/home-page/home-page.component';
import { RoomsPageComponent } from './pages/rooms-page/rooms-page.component';
import { ContactPageComponent } from './pages/contact-page/contact-page.component';
import { BookingsComponent } from './pages/bookings/bookings.component';
import { FormsModule } from '@angular/forms';

@NgModule({
  declarations: [
    AppComponent,
    HomePageComponent,
    RoomsPageComponent,
    ContactPageComponent,
    BookingsComponent,
  ],
  imports: [BrowserModule, AppRoutingModule, AdminModule, FormsModule],
  providers: [
    provideHttpClient(withInterceptorsFromDi()),
    { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: HttpErrorInterceptor, multi: true },
  ],
  bootstrap: [AppComponent],
})
export class AppModule {}
