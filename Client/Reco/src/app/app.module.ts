import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppComponent } from './app.component';
import { HomeComponent } from './home/home.component';
import {DataService} from './data.service';
import { HttpClientModule } from '@angular/common/http';

import {BrowserAnimationsModule} from '@angular/platform-browser/animations';
import {MatToolbarModule, MatSelectModule,MatGridListModule, MatCardModule} from '@angular/material';


@NgModule({
  declarations: [
    AppComponent,
    HomeComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    BrowserAnimationsModule,
    MatToolbarModule,
    MatSelectModule,
    MatGridListModule,
    MatCardModule
  ],
  providers: [DataService],
  bootstrap: [AppComponent]
})
export class AppModule { }
