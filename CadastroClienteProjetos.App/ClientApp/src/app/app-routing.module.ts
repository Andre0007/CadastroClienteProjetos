import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ClienteComponent } from '../Pages/Cliente/cliente-component';
import { HomeComponent } from '../Pages/Home/home-component';
import { ProjetoComponent } from '../Pages/Projeto/projeto-component';

const routes: Routes = [
  {
    path: 'home',
    component: HomeComponent
  },{
    path: 'cliente',
    component: ClienteComponent
  }, {
    path: 'projeto',
    component: ProjetoComponent
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
