import { ComponentFixture, TestBed } from '@angular/core/testing';

import { VenderlistComponent } from './venderlist.component';

describe('VenderlistComponent', () => {
  let component: VenderlistComponent;
  let fixture: ComponentFixture<VenderlistComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ VenderlistComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(VenderlistComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
