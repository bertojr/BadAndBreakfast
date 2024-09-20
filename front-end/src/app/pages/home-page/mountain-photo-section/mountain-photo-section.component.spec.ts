import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MountainPhotoSectionComponent } from './mountain-photo-section.component';

describe('MountainPhotoSectionComponent', () => {
  let component: MountainPhotoSectionComponent;
  let fixture: ComponentFixture<MountainPhotoSectionComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [MountainPhotoSectionComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(MountainPhotoSectionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
