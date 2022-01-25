-- public.items definition

-- Drop table

-- DROP TABLE public.items;

CREATE TABLE IF NOT EXISTS public.items
(
    id               text      NOT NULL,
    display_name     text      NOT NULL,
    description      text      NOT NULL,
    price            float8    NOT NULL,
    "type"           int4      NOT NULL,
    "data"           json      NOT NULL,
    is_enabled       bool      NOT NULL DEFAULT true,
    available_since  timestamp NOT NULL DEFAULT now(),
    available_before timestamp NULL,
    CONSTRAINT items_pk PRIMARY KEY (id)
);
CREATE INDEX IF NOT EXISTS items_description_idx ON public.items USING btree (description);
CREATE INDEX IF NOT EXISTS items_display_name_idx ON public.items USING btree (display_name);


-- public.clients definition

-- Drop table

-- DROP TABLE public.clients;

CREATE TABLE IF NOT EXISTS public.clients
(
    id           text NOT NULL,
    first_name   text NOT NULL,
    second_name  text NOT NULL,
    email        text NOT NULL,
    phone_number text NOT NULL,
    CONSTRAINT clients_pk PRIMARY KEY (id),
    CONSTRAINT clients_un UNIQUE (email)
);
CREATE INDEX IF NOT EXISTS clients_email_idx ON public.clients USING btree (email);
CREATE INDEX IF NOT EXISTS clients_first_name_idx ON public.clients USING btree (first_name);
CREATE INDEX IF NOT EXISTS clients_phone_number_idx ON public.clients USING btree (phone_number);
CREATE INDEX IF NOT EXISTS clients_second_name_idx ON public.clients USING btree (second_name);


-- public.client_items definition

-- Drop table

-- DROP TABLE public.client_items;

CREATE TABLE IF NOT EXISTS public.client_items
(
    id                           text      NOT NULL,
    client_id                    text      NOT NULL,
    item_id                      text      NOT NULL,
    payment_system_bill_id       text      NULL,
    payment_system_bill_due_date timestamp NULL,
    is_paid                      bool      NOT NULL DEFAULT false,
    is_processed                 bool      NOT NULL DEFAULT false,
    "data"                       json      NULL,
    payment_date                 timestamp NULL,
    process_date                 timestamp NULL,
    CONSTRAINT bills_pk PRIMARY KEY (id),
    CONSTRAINT bills_fk FOREIGN KEY (item_id) REFERENCES public.items (id) ON UPDATE CASCADE,
    CONSTRAINT bills_fk_1 FOREIGN KEY (client_id) REFERENCES public.clients (id) ON UPDATE CASCADE
);
CREATE INDEX IF NOT EXISTS bills_client_id_idx ON public.client_items USING btree (client_id);
CREATE INDEX IF NOT EXISTS bills_item_id_idx ON public.client_items USING btree (item_id);
CREATE INDEX IF NOT EXISTS bills_payment_system_id_idx ON public.client_items USING btree (payment_system_bill_id);
CREATE INDEX IF NOT EXISTS payment_system_bill_due_date_idx ON public.client_items USING btree (payment_system_bill_due_date);