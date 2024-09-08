import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SideNavComponent } from './side-nav/side-nav.component';
import { TopNavComponent } from './top-nav/top-nav.component';
import { ContentHeaderComponent } from './content-header/content-header.component';
import { ContentFooterComponent } from './content-footer/content-footer.component';
import { RouterModule } from '@angular/router';
import { DateTimeControlsDirective } from './directives/date-time-controls.directive';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NotfoundComponent } from './notfound/notfound.component';


@NgModule({
  declarations: [
    SideNavComponent,
    TopNavComponent,
    ContentHeaderComponent,
    ContentFooterComponent,
    DateTimeControlsDirective,
    NotfoundComponent,
  ],
  imports: [
    CommonModule,
    RouterModule,
    // BrowserAnimationsModule,
    BsDropdownModule.forRoot()
  ],
  exports: [
    SideNavComponent,
    TopNavComponent,
    ContentHeaderComponent,
    ContentFooterComponent,
    DateTimeControlsDirective,
    NotfoundComponent
  ]
})
export class CoreModule { }
