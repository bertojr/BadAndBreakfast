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
  checkInDate: Date | null = null;
  checkOutDate: Date | null = null;

  constructor(http: HttpClient) {
    super(http);
    this.setEndpoint('Booking');
  }

  getAvailableRooms(): Observable<iRoom[]> {
    const apiUrl = this.getUrl();

    return this.http.post<iRoom[]>(`${apiUrl}/search`, {
      checkInDate: this.checkInDate,
      checkOutDate: this.checkOutDate,
    });
  }

  setDates(checkIn: Date, checkOut: Date): void {
    this.checkInDate = checkIn;
    this.checkOutDate = checkOut;
  }

  getDates(): { checkInDate: Date | null; checkOutDate: Date | null } {
    return { checkInDate: this.checkInDate, checkOutDate: this.checkOutDate };
  }
}
