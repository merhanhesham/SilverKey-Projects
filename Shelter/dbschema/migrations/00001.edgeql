CREATE MIGRATION m1fp2du5lwnw3ysfxwh6suuicwvj5h4xvu24vu7fwa3imen4zgd6eq
    ONTO initial
{
  CREATE TYPE default::Bathroom {
      CREATE REQUIRED PROPERTY area: std::decimal;
      CREATE REQUIRED PROPERTY hot_water: std::bool;
      CREATE REQUIRED PROPERTY is_shared: std::bool;
      CREATE REQUIRED PROPERTY washing_machine: std::bool;
  };
  CREATE TYPE default::Bedroom {
      CREATE REQUIRED PROPERTY air_conditioner: std::bool;
      CREATE REQUIRED PROPERTY area: std::decimal;
      CREATE REQUIRED PROPERTY balcony: std::bool;
      CREATE REQUIRED PROPERTY fans: std::bool;
      CREATE REQUIRED PROPERTY has_private_bathroom: std::bool;
      CREATE REQUIRED PROPERTY number_of_double_beds: std::int16;
      CREATE REQUIRED PROPERTY number_of_single_beds: std::int16;
      CREATE REQUIRED PROPERTY wardrobe: std::bool;
      CREATE REQUIRED PROPERTY workspaces: std::int16;
  };
  CREATE TYPE default::Kitchen {
      CREATE REQUIRED PROPERTY cooker: std::bool;
      CREATE REQUIRED PROPERTY dishes_and_silverware: std::bool;
      CREATE REQUIRED PROPERTY gas_tube: std::bool;
      CREATE REQUIRED PROPERTY household_gas: std::bool;
      CREATE REQUIRED PROPERTY kitchen_area: std::decimal;
      CREATE REQUIRED PROPERTY microwave: std::bool;
      CREATE REQUIRED PROPERTY refrigerator: std::bool;
      CREATE REQUIRED PROPERTY washer: std::bool;
      CREATE REQUIRED PROPERTY water_kettle: std::bool;
  };
  CREATE TYPE default::Livingroom {
      CREATE REQUIRED PROPERTY air_conditioner: std::bool;
      CREATE REQUIRED PROPERTY balcony: std::bool;
      CREATE REQUIRED PROPERTY dining_table: std::bool;
      CREATE REQUIRED PROPERTY fans: std::bool;
      CREATE REQUIRED PROPERTY livingroom_area: std::decimal;
      CREATE REQUIRED PROPERTY number_of_livingroom: std::int16;
      CREATE REQUIRED PROPERTY number_of_sofas: std::int16;
      CREATE REQUIRED PROPERTY tv: std::bool;
  };
  CREATE TYPE default::Apartment {
      CREATE REQUIRED LINK bathroom: default::Bathroom;
      CREATE MULTI LINK bedroom: default::Bedroom;
      CREATE REQUIRED LINK kitchen: default::Kitchen;
      CREATE MULTI LINK living_room: default::Livingroom;
      CREATE REQUIRED PROPERTY number_of_bedrooms: std::int16;
      CREATE REQUIRED PROPERTY number_of_livingrooms: std::int16;
  };
  CREATE TYPE default::Place {
      CREATE REQUIRED PROPERTY city: std::str;
      CREATE PROPERTY description: std::str;
      CREATE REQUIRED PROPERTY elevator: std::bool;
      CREATE REQUIRED PROPERTY governorate: std::str;
      CREATE REQUIRED PROPERTY grab_bars: std::bool;
      CREATE PROPERTY images: array<std::str>;
      CREATE REQUIRED PROPERTY is_reserved: std::bool;
      CREATE REQUIRED PROPERTY pets_allowed: std::bool;
      CREATE REQUIRED PROPERTY place_type: std::str;
      CREATE REQUIRED PROPERTY price: std::decimal;
      CREATE REQUIRED PROPERTY private_parking: std::bool;
      CREATE REQUIRED PROPERTY rental_type: std::str;
      CREATE REQUIRED PROPERTY tenants: std::int16;
      CREATE REQUIRED PROPERTY title: std::str;
      CREATE PROPERTY total_area: std::decimal;
      CREATE REQUIRED PROPERTY wheelchair: std::bool;
      CREATE REQUIRED PROPERTY wifi: std::bool;
  };
  ALTER TYPE default::Apartment {
      CREATE REQUIRED LINK place: default::Place;
  };
  CREATE TYPE default::House {
      CREATE MULTI LINK bathroom: default::Bathroom;
      CREATE MULTI LINK bedroom: default::Bedroom;
      CREATE REQUIRED LINK kitchen: default::Kitchen;
      CREATE MULTI LINK number_of_livingrooms: default::Livingroom;
      CREATE REQUIRED LINK place: default::Place;
      CREATE REQUIRED PROPERTY number_of_bathrooms: std::int16;
      CREATE REQUIRED PROPERTY number_of_bedrooms: std::int16;
      CREATE REQUIRED PROPERTY number_of_floors: std::int16;
      CREATE REQUIRED PROPERTY number_of_livingroom: std::int16;
  };
  CREATE TYPE default::Room {
      CREATE REQUIRED LINK bathroom: default::Bathroom;
      CREATE REQUIRED LINK kitchen: default::Kitchen;
      CREATE REQUIRED LINK place: default::Place;
      CREATE REQUIRED PROPERTY balcony: std::bool;
      CREATE REQUIRED PROPERTY has_air_conditioner: std::bool;
      CREATE REQUIRED PROPERTY has_wardrobe: std::bool;
      CREATE REQUIRED PROPERTY has_workspace: std::bool;
      CREATE REQUIRED PROPERTY number_of_beds: std::int16;
      CREATE REQUIRED PROPERTY room_area: std::decimal;
      CREATE REQUIRED PROPERTY shared_bathroom: std::bool;
      CREATE REQUIRED PROPERTY tv: std::bool;
  };
  CREATE TYPE default::LongTerm {
      CREATE REQUIRED LINK place: default::Place;
      CREATE REQUIRED PROPERTY currency: std::str;
      CREATE REQUIRED PROPERTY price_per_month: std::decimal;
      CREATE REQUIRED PROPERTY price_per_year: std::decimal;
  };
  CREATE TYPE default::User {
      CREATE MULTI LINK owns_properties: default::Place;
      CREATE REQUIRED PROPERTY email: std::str;
      CREATE REQUIRED PROPERTY first_name: std::str;
      CREATE PROPERTY gender: std::str;
      CREATE REQUIRED PROPERTY has_pets: std::bool;
      CREATE REQUIRED PROPERTY is_smoker: std::bool;
      CREATE REQUIRED PROPERTY last_name: std::str;
      CREATE REQUIRED PROPERTY password: std::str;
      CREATE PROPERTY pets_type: std::str;
      CREATE PROPERTY phone: std::str;
      CREATE REQUIRED PROPERTY user_type: std::str;
  };
  ALTER TYPE default::Place {
      CREATE REQUIRED LINK landlord: default::User;
      CREATE MULTI LINK tenant_id: default::User;
  };
  CREATE TYPE default::ShortTerm {
      CREATE REQUIRED LINK place: default::Place;
      CREATE REQUIRED PROPERTY currency: std::str;
      CREATE REQUIRED PROPERTY price_per_night: std::decimal;
      CREATE REQUIRED PROPERTY reserve_from: std::datetime;
      CREATE REQUIRED PROPERTY reserve_to: std::datetime;
  };
  CREATE TYPE default::Rating {
      CREATE REQUIRED LINK rated_place: default::Place;
      CREATE REQUIRED LINK reviewer: default::User;
      CREATE REQUIRED PROPERTY cleanliness_rating: std::int16;
      CREATE REQUIRED PROPERTY comment: std::str;
      CREATE REQUIRED PROPERTY communication_rating: std::int16;
      CREATE REQUIRED PROPERTY location: std::int16;
      CREATE PROPERTY rating_date: std::datetime;
  };
};
