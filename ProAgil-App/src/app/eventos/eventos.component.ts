import { Component, OnInit, TemplateRef } from '@angular/core';
import { EventoService } from '../_services/evento.service';
import { Evento } from '../_models/Evento';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { typeWithParameters } from '@angular/compiler/src/render3/util';

@Component({
  selector: 'app-eventos',
  templateUrl: './eventos.component.html',
  styleUrls: ['./eventos.component.css']
})
export class EventosComponent implements OnInit {
  eventosFiltrados: Evento[] = [];
  eventos: Evento[] = [];
  imagemMargem = 2;
  imagemLargura = 50;
  mostrarImagem = false;
  modalRef: BsModalRef;
  registerForm: FormGroup;

  // tslint:disable-next-line: variable-name
  private _filtroLista: string;

  constructor(
    private eventoService: EventoService,
    private modalService: BsModalService
  ) {}

  get filtroLista(): string {
    return this._filtroLista;
  }

  set filtroLista(value: string) {
    this._filtroLista = value;
    this.eventosFiltrados = this.filtroLista
      ? this.filtrarEventos(this.filtroLista)
      : this.eventos;
  }

  openModal(template: TemplateRef<any>) {
    this.modalRef = this.modalService.show(template);
  }

  filtrarEventos(filtro: string): Evento[] {
    filtro = filtro.toLocaleLowerCase();
    return this.eventos.filter(
      evento => evento.Tema.toLocaleLowerCase().indexOf(filtro) !== -1
    );
  }

  ngOnInit() {
    this.getEventos();
    this.validation();
  }

  alternarImagem() {
    this.mostrarImagem = !this.mostrarImagem;
  }

  validation() {
    this.registerForm = new FormGroup({
      Tema: new FormControl('', [
        Validators.required,
        Validators.minLength(4),
        Validators.maxLength(50)
      ]),
      Local: new FormControl('', Validators.required),
      DataEvento: new FormControl('', Validators.required),
      QtdePessoa: new FormControl('', [
        Validators.required,
        Validators.min(1),
        Validators.max(120)
      ]),
      ImagemUrl: new FormControl('', Validators.required),
      Telefone: new FormControl('', Validators.required),
      Email: new FormControl('', [Validators.required, Validators.email])
    });
  }

  salvarAlteracao() {}

  getEventos() {
    this.eventoService.getAllEventos().subscribe(
      (eventos: Evento[]) => {
        this.eventos = eventos;
        this.eventosFiltrados = eventos;
        console.log(eventos);
      },
      error => {
        console.log(error);
      }
    );
  }
}
