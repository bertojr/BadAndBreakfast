import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { BookingService } from '../../services/booking.service';

@Component({
  selector: 'app-home-page',
  templateUrl: './home-page.component.html',
  styleUrl: './home-page.component.scss',
})
export class HomePageComponent {
  checkInDate!: string;
  checkOutDate!: string;

  constructor(private router: Router, private bookingSvc: BookingService) {}

  ngOnInit(): void {
    const dates = this.bookingSvc.getDates();
    this.checkInDate = dates.checkInDate;
    this.checkOutDate = dates.checkOutDate;
  }
  search() {
    if (this.checkInDate && this.checkOutDate) {
      this.bookingSvc.setDates(
        new Date(this.checkInDate),
        new Date(this.checkOutDate)
      );
      this.router.navigate(['/bookings']);
    }
  }
}
