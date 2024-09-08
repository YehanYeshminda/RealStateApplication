import { ComponentFixture, TestBed } from '@angular/core/testing';

import { IouComponent } from './iou.component';

describe('IouComponent', () => {
  let component: IouComponent;
  let fixture: ComponentFixture<IouComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [IouComponent]
    });
    fixture = TestBed.createComponent(IouComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
