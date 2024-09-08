import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CampaindetailslistComponent } from './campaindetailslist.component';

describe('CampaindetailslistComponent', () => {
  let component: CampaindetailslistComponent;
  let fixture: ComponentFixture<CampaindetailslistComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [CampaindetailslistComponent]
    });
    fixture = TestBed.createComponent(CampaindetailslistComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
