CREATE MIGRATION m16spqi4qgoypswfqdacixgpbyevm7cpg3jre7d72ydm6jtgcalmzq
    ONTO m16smqiffrfejvy3augh3t5gv6entzyigx3h46sbvcsmsajdtgc6cq
{
  CREATE TYPE default::Person {
      CREATE REQUIRED PROPERTY name: std::str;
  };
  CREATE TYPE default::Movie {
      CREATE MULTI LINK actors: default::Person;
      CREATE PROPERTY title: std::str;
  };
};
