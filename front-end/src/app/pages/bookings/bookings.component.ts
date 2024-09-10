import { Component } from '@angular/core';
import { BookingService } from '../../services/booking.service';
import { iRoom } from '../../models/i-room';

@Component({
  selector: 'app-bookings',
  templateUrl: './bookings.component.html',
  styleUrl: './bookings.component.scss',
})
export class BookingsComponent {
  checkInDate: Date | null = null;
  checkOutDate: Date | null = null;
  errorMessage: string | null = null;
  availableRooms!: iRoom[];
  constructor(private bookingSvc: BookingService) {}

  ngOnInit() {
    const date = this.bookingSvc.getDates();
    this.checkInDate = date.checkInDate;
    this.checkOutDate = date.checkOutDate;

    this.getAvailableRooms();
  }

  getAvailableRooms() {
    if (this.checkInDate && this.checkOutDate) {
      this.bookingSvc.getAvailableRooms().subscribe({
        next: (rooms) => {
          this.availableRooms = rooms;
          this.errorMessage = null;
        },
        error: (error) => (this.errorMessage = error.message),
      });
    }
  }
  search() {
    this.getAvailableRooms();
  }
}
