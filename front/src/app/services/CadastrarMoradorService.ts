  // cadastrar-morador.service.ts
  import { Injectable } from '@angular/core';
  import { HttpClient } from '@angular/common/http';
  import { Observable } from 'rxjs';
  import { CadastrarMoradoresComponent } from '../components/cadastrar-moradores/cadastrar-moradores.component'; // Ajuste o caminho conforme necessário
  import { MoradoresComponent } from '../components/moradores/moradores.component';

  @Injectable({
    providedIn: 'root'
  })
  export class CadastrarMoradorService {
    private URL: string = 'http://localhost:5134/moradores'; // URL da sua API

    constructor(private http: HttpClient) { }

    cadastrar(morador: MoradoresComponent): Observable<MoradoresComponent> {
      return this.http.post<MoradoresComponent>(this.URL, morador);
    }
  }