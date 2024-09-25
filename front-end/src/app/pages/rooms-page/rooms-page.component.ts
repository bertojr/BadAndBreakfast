import { Component } from '@angular/core';
import { iRoom } from '../../models/i-room';
import { RoomService } from '../../services/room.service';

@Component({
  selector: 'app-rooms-page',
  templateUrl: './rooms-page.component.html',
  styleUrl: './rooms-page.component.scss',
})
export class RoomsPageComponent {
  rooms!: iRoom[];
  errorMessage: string | null = null;

  constructor(private roomSvc: RoomService) {}

  scrollToBookings(): void {
    const bookingElement = document.getElementById('bookings');
    if (bookingElement) {
      bookingElement.scrollIntoView({ behavior: 'smooth' });
    }
  }
  ngOnInit(): void {
    this.loadRooms();
  }

  loadRooms(): void {
    this.roomSvc.getAll().subscribe({
      next: (rooms) => {
        this.rooms = rooms;
        this.errorMessage = null;
      },
      error: (error) => {
        this.errorMessage = error.message;
      },
    });
  }
}
