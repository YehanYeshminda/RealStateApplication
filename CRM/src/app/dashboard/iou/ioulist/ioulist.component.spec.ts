import { ComponentFixture, TestBed } from '@angular/core/testing';

import { IoulistComponent } from './ioulist.component';

describe('IoulistComponent', () => {
  let component: IoulistComponent;
  let fixture: ComponentFixture<IoulistComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [IoulistComponent]
    });
    fixture = TestBed.createComponent(IoulistComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
