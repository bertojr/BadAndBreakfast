import { iRoom } from './../../models/i-room';
import { Component } from '@angular/core';
import { BookingService } from '../../services/booking.service';
import { iBooking } from '../../models/i-booking';

@Component({
  selector: 'app-bookings',
  templateUrl: './bookings.component.html',
  styleUrl: './bookings.component.scss',
})
export class BookingsComponent {
  checkInDate!: string;
  checkOutDate!: string;
  checkInDateFormatted!: string;
  checkOutDateFormatted!: string;
  errorMessage: string | null = null;
  availableRooms!: iRoom[];
  selectedRooms: iRoom[] = [];
  guests: number = 2;
  constructor(private bookingSvc: BookingService) {}

  ngOnInit() {
    this.updateDates();
    this.loadAvailableRooms();
  }

  private updateDates(): void {
    const dates = this.bookingSvc.getDates();
    this.checkInDate = dates.checkInDate;
    this.checkOutDate = dates.checkOutDate;
    const datesFormatted = this.bookingSvc.getDatesFormatted();
    this.checkInDateFormatted = datesFormatted.checkInDate;
    this.checkOutDateFormatted = datesFormatted.checkOutDate;
  }
  private loadAvailableRooms() {
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
    this.bookingSvc.setDates(
      new Date(this.checkInDate),
      new Date(this.checkOutDate)
    );
    this.updateDates();
    this.loadAvailableRooms();
  }

  toogleSelection(room: iRoom) {
    const index = this.selectedRooms.findIndex(
      (selected) => selected.roomID === room.roomID
    );

    if (index > -1) {
      this.selectedRooms.splice(index, 1);
    } else {
      this.selectedRooms.push(room);
    }
  }

  isSelected(roomId: number): boolean {
    return this.selectedRooms.some((room) => room.roomID === roomId);
  }

  getTotalRooms(): number {
    return this.selectedRooms.length;
  }

  getDailyPrice(): number {
    return this.selectedRooms.reduce((sum, room) => sum + room.price, 0);
  }

  getNumberOfNights(): number {
    const checkIn = new Date(this.checkInDate);
    const checkOut = new Date(this.checkOutDate);

    // differenza in millisecondi tra checkin e checkout
    const diffInTime = checkOut.getTime() - checkIn.getTime();

    // conversione da millisecondi a giorni
    const diffInDays = diffInTime / (1000 * 3600 * 24);

    return diffInDays;
  }

  getTotal(): number {
    return this.getDailyPrice() * this.getNumberOfNights();
  }

  toBook() {
    const newBook = {
      checkInDate: this.checkInDate,
      checkOutDate: this.checkOutDate,
      numberOfGuests: this.guests,
      roomIds: this.selectedRooms.map((room) => room.roomID),
      serviceIds: null,
      specialRequests: null,
    };

    this.bookingSvc.create(newBook).subscribe({
      next: () => {
        this.errorMessage = null;
      },
      error: (error) => (this.errorMessage = error.message),
    });
  }
}
