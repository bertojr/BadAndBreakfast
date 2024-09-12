import { iAdditionalService } from './i-additional-service';
import { iRoom } from './i-room';
import { iUser } from './i-user';

export interface iBooking {
  bookingID: number;
  checkInDate: string;
  checkOutDate: string;
  totalPrice: number;
  status: string;
  paymentStatus: string;
  bookingDate: string;
  numberOfGuest: number;
  specialRequest?: string;
  user: iUser;
  rooms: iRoom[];
  additionalServices: iAdditionalService[];
}
