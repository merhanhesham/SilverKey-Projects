CREATE MIGRATION m1nyaihvekg4re4wdzikksr4xa6onsjzzb324iai7yvmqdnlzlqi7a
    ONTO m16spqi4qgoypswfqdacixgpbyevm7cpg3jre7d72ydm6jtgcalmzq
{
  ALTER TYPE default::Movie {
      ALTER PROPERTY title {
          SET REQUIRED USING ('Untitled');
      };
  };
};
