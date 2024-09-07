import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { AmenityService } from '../../../services/amenity.service';
import { RoomService } from '../../../services/room.service';
import { iAmenity } from '../../../models/i-amenity';
import { iRoom } from '../../../models/i-room';

@Component({
  selector: 'app-room-edit',
  templateUrl: './room-edit.component.html',
  styleUrl: './room-edit.component.scss',
})
export class RoomEditComponent {
  amenities: iAmenity[] = [];
  errorMessage: string | null = '';
  room!: iRoom;
  roomForm!: FormGroup;
  selectedAmenityId: number[] = [];
  selectedAmenity: number | null = null;

  constructor(
    private route: ActivatedRoute,
    private roomSvc: RoomService,
    private amenitySvc: AmenityService,
    private fb: FormBuilder
  ) {}

  ngOnInit(): void {
    // inizializzo il form
    this.initializeForm();

    // Caricare tutte le comodità per la selezione
    this.loadAmenities();

    // Controlla se stiamo modificando una camera esistente
    this.route.params.subscribe((params: any) => {
      const roomId = params.id;
      if (roomId) {
        this.loadRoom(roomId);
      }
    });
  }

  private initializeForm(): void {
    this.roomForm = this.fb.group({
      roomNumber: this.fb.control(null, Validators.required),
      capacity: this.fb.control(null, Validators.required),
      description: this.fb.control(null, Validators.required),
      isAvailable: this.fb.control(true),
      price: this.fb.control(null, Validators.required),
      size: this.fb.control(null, Validators.required),
      roomType: this.fb.control(null, Validators.required),
      amenitiesIds: this.fb.control([]),
    });
  }

  private populateForm(room: iRoom): void {
    this.roomForm.patchValue({
      roomNumber: room.roomNumber,
      capacity: room.capacity,
      description: room.description,
      isAvailable: room.isAvailable,
      price: room.price,
      size: room.size,
      roomType: room.roomType,
    });
  }
  private loadAmenities() {
    this.amenitySvc.getAll().subscribe({
      next: (amenities) => {
        this.amenities = amenities;
        this.errorMessage = null;
      },
      error: (error) => {
        this.errorMessage =
          error.message || 'Errore durante il caricamento delle comodità';
      },
    });
  }

  onSubmit(): void {
    if (this.roomForm.valid) {
      const roomData: iRoom = this.roomForm.value;
      if (this.room) {
        // Modifica esistente
        this.roomSvc.update(this.room.roomID, roomData).subscribe({
          next: () => {
            this.errorMessage = null;
          },
          error: (error) => {
            this.errorMessage =
              error.message || "Errore durante l'aggiornamento della camera";
          },
        });
      } else {
        // Nuova creazione
        this.roomSvc.create(roomData).subscribe({
          next: () => {
            this.errorMessage = null;
          },
          error: (error) => {
            this.errorMessage =
              error.message || 'Errore durante la creazione della camera';
          },
        });
      }
    }
  }
  private loadRoom(roomId: number) {
    this.roomSvc.getById(roomId).subscribe({
      next: (room) => {
        this.errorMessage = null;
        this.room = room;
        this.populateForm(room);
      },
      error: (error) => {
        this.errorMessage =
          error.message || 'Errore durante il caricamento della camera';
      },
    });
  }

  addAmenity(): void {
    if (this.selectedAmenity !== null) {
      this.roomSvc
        .addAmenityToRoom(this.room.roomID, this.selectedAmenity)
        .subscribe({
          next: (amenity) => {
            this.room.amenities?.push(amenity);
            this.errorMessage = null;
          },
          error: (error) => {
            this.errorMessage =
              error.message || 'Si è verificato un errore durante la ricerca';
          },
        });
    }
  }

  removeAmenity(roomId: number, amenityId: number): void {
    this.roomSvc.removeAmenityFromRoom(roomId, amenityId).subscribe({
      next: () => {
        this.room.amenities = this.room.amenities?.filter(
          (a) => a.amenityID !== amenityId
        );
        this.errorMessage = null;
      },
      error: (error) => {
        this.errorMessage =
          error.message || 'Si è verificato un errore durante la ricerca';
      },
    });
  }
}
