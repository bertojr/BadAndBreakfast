import { Component } from '@angular/core';
import { iRoom } from '../../../models/i-room';
import { RoomService } from '../../../services/room.service';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { RoomEditComponent } from '../room-edit/room-edit.component';

@Component({
  selector: 'app-room-list',
  templateUrl: './room-list.component.html',
  styleUrl: './room-list.component.scss',
})
export class RoomListComponent {
  rooms: iRoom[] = [];
  errorMessage: string | null = '';

  constructor(private roomSvc: RoomService, private modalSvc: NgbModal) {}

  ngOnInit(): void {
    this.loadRooms();
  }

  private loadRooms() {
    this.roomSvc.getAll().subscribe({
      next: (rooms) => {
        this.rooms = rooms;
        this.errorMessage = null;
      },
      error: (error) => {
        this.errorMessage =
          error.message || 'Si è verificato un errore durante la ricerca';
      },
    });
  }

  deleteRoom(id: number): void {
    this.roomSvc.delete(id).subscribe({
      next: () => {
        this.rooms = this.rooms.filter((room) => room.roomID !== id);
        this.errorMessage = null;
      },
      error: (error) => {
        this.errorMessage =
          error.message || 'Si è verificato un errore durante la eliminazione';
      },
    });
  }

  openModal(room?: iRoom) {
    const modalRef = this.modalSvc.open(RoomEditComponent);
    modalRef.componentInstance.room = room;

    modalRef.componentInstance.roomUpdated.subscribe(() => {
      this.loadRooms();
    });
  }
}
