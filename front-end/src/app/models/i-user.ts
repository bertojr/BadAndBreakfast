import { iBooking } from './i-booking';
import { iReview } from './i-review';
import { iRole } from './i-role';

export interface iUser {
  userID: number;
  name?: string;
  email: string;
  cell?: string;
  dateOfBirth?: string;
  nationally?: string;
  gender?: string;
  password: string;
  country?: string;
  address?: string;
  city?: string;
  cap?: string;
  reviews?: iReview[];
  roles?: iRole[];
  bookings: iBooking[];
}
