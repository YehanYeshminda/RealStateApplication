import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CampaindetailsComponent } from './campaindetails.component';

describe('CampaindetailsComponent', () => {
  let component: CampaindetailsComponent;
  let fixture: ComponentFixture<CampaindetailsComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [CampaindetailsComponent]
    });
    fixture = TestBed.createComponent(CampaindetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
