export interface Evento {
  Id: number;
  Local: string;
  DataEvento: Date;
  Tema: string;
  QtdePessoas: number;
  ImagemUrl: string;
  Telefone: string;
  Email: string;
  Lote: string;
  // Lotes: Lote[];
  // RedesSociais: RedeSocial[];
  // PalestranteEventos: Palestrante[];
}
