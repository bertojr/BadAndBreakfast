import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AmenityEditComponent } from './amenity-edit.component';

describe('AmenityEditComponent', () => {
  let component: AmenityEditComponent;
  let fixture: ComponentFixture<AmenityEditComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [AmenityEditComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AmenityEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
