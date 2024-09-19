import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AmenityListComponent } from './amenity-list.component';

describe('AmenityListComponent', () => {
  let component: AmenityListComponent;
  let fixture: ComponentFixture<AmenityListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [AmenityListComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AmenityListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
