import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { AppRoutingModule } from './app-routing.module';
import { ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { SlimLoadingBarModule } from 'ng2-slim-loading-bar';
import { AppComponent } from './app.component';
import { NgxMaskModule } from 'ngx-mask';
import { NgxPaginationModule } from 'ngx-pagination';
import { ChartsModule } from 'ng2-charts';

//Pages Components
import { HomeComponent } from '../Pages/Home/home-component';
import { ClienteComponent } from '../Pages/Cliente/cliente-component';
import { ProjetoComponent } from '../Pages/Projeto/projeto-component';

//Services
import { HomeService } from '../service/home.service';
import { ClienteService } from '../service/cliente.service';
import { ProjetoService } from '../service/projeto.service';

export const options: Partial<NgxMaskModule> | (() => Partial<NgxMaskModule>) = {};

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    ClienteComponent,
    ProjetoComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    SlimLoadingBarModule,
    ReactiveFormsModule,
    HttpClientModule,
    NgxPaginationModule,
    ChartsModule,
    NgxMaskModule.forRoot(options)
  ],
  providers: [HomeService, ClienteService, ProjetoService],
  bootstrap: [AppComponent]
})
export class AppModule { }
