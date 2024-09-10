import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { BookingService } from '../../services/booking.service';

@Component({
  selector: 'app-home-page',
  templateUrl: './home-page.component.html',
  styleUrl: './home-page.component.scss',
})
export class HomePageComponent {
  checkInDate: Date | null = null;
  checkOutDate: Date | null = null;

  constructor(private router: Router, private bookingSvc: BookingService) {}

  search() {
    if (this.checkInDate && this.checkOutDate) {
      this.bookingSvc.setDates(this.checkInDate, this.checkOutDate);
      this.router.navigate(['/bookings']);
    }
  }
}
