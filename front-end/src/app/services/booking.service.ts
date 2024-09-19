import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { iRoom } from '../models/i-room';
import { Observable } from 'rxjs';
import { CrudService } from './crud.service';
import { iBooking } from '../models/i-booking';

@Injectable({
  providedIn: 'root',
})
export class BookingService extends CrudService<iBooking> {
  checkInDate!: Date;
  checkOutDate!: Date;

  constructor(http: HttpClient) {
    super(http);
    this.setEndpoint('Booking');
    this.initializeDates();
  }

  initializeDates(): void {
    this.checkInDate = new Date();
    this.checkOutDate = new Date(new Date().setDate(new Date().getDate() + 7));
  }

  getAvailableRooms(): Observable<iRoom[]> {
    const apiUrl = this.getUrl();

    return this.http.post<iRoom[]>(`${apiUrl}/search`, {
      checkInDate: this.formatDate(this.checkInDate),
      checkOutDate: this.formatDate(this.checkOutDate),
    });
  }

  setDates(checkIn: Date, checkOut: Date): void {
    this.checkInDate = checkIn;
    this.checkOutDate = checkOut;
  }

  getDates(): { checkInDate: string; checkOutDate: string } {
    return {
      checkInDate: this.formatDate(this.checkInDate),
      checkOutDate: this.formatDate(this.checkOutDate),
    };
  }

  getDatesFormatted(): { checkInDate: string; checkOutDate: string } {
    return {
      checkInDate: this.formattedDate(this.checkInDate),
      checkOutDate: this.formattedDate(this.checkOutDate),
    };
  }

  private formatDate(date: Date): string {
    return date.toISOString().split('T')[0];
  }

  private formattedDate(date: Date, locale: string = 'it-IT'): string {
    const options: Intl.DateTimeFormatOptions = {
      weekday: 'short',
      day: '2-digit',
      month: 'short',
      year: 'numeric',
    };
    return new Intl.DateTimeFormat(locale, options).format(date); // Format for display (e.g., 'mar, 17 set 2024')
  }
}
