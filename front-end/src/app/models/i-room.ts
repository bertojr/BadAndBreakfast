import { iAmenity } from './i-amenity';
import { iRoomImage } from './i-room-image';

export interface iRoom {
  roomID: number;
  roomNumber: number;
  roomType: string;
  capacity: number;
  price: number;
  description: string;
  isAvailable?: boolean;
  size: string;
  roomImages?: iRoomImage[];
  amenities?: iAmenity[];
  amenitiesIds?: number;
}
